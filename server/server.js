var net = require('net');
var base64 = require('base-64');
var crypto = require('crypto');
var bignum = require('big-integer');
var notifSender = require('./notifications');

function string_to_buffer(str)
{
	buffer = Buffer.alloc(str.length);
	for (let i = 0; i < str.length; i++)
		buffer[i] = str.charCodeAt(i);
	return buffer;
}

var security = {
	hexify: function(s)
	{
		let hex = [ ];
		for (let i = 0; i < s.length; i++)
		{
			let c = s[i].toString(16);
			hex.push(c.length == 2 ? c : '0'+c);
		}
		return hex.join('');
	},
	checksum: function(data)
	{
		var sha1 = crypto.createHash('sha1');
		var md5 = crypto.createHash('md5');
		md5.update("<nekess_nedjima>" + data + "</nekess_nedjima>");
		sha1.update(md5.digest());
		return security.remove(sha1.digest('base64'), "\n");
	},
	encrypt: function(data, key)
	{
		if (typeof key === 'undefined')
		{
			throw "[-] DH wasn't completed and encrypt was called";
		}
		else
		{
			let iv = crypto.randomBytes(16);
			let aes = crypto.createCipheriv('aes-256-cbc', key, iv);
			aes.update(data);
			let enc = security.remove(aes.final().toString('base64'), "\n");
			let iv_enc = security.remove(iv.toString('base64'), "\n");
			console.log("enc : iv = " + security.hexify(iv) + " ; key = " + security.hexify(key));
			return enc + '$' + iv_enc + '$' + this.checksum(enc + iv_enc);
		}
	},
	decrypt: function(s,key)
	{	
		console.log("s = " + s);
		let results = s.toString().split('$');
		let chksum = results[2].trim();
		if (security.checksum(results[0] + results[1]) !== chksum)
		{
			throw "Integrity Error"; // fix integrity check
		}
		
		let body = string_to_buffer(base64.decode(results[0]));
		console.log("body = " + security.hexify(body));
		let str_iv = base64.decode(results[1]);
		let iv = string_to_buffer(str_iv);
		console.log("iv = " + iv.toString('hex'));
		let aes = crypto.createDecipheriv('aes-256-cbc', key, iv);
		return aes.update(body) + aes.final();
	},
	integre: function(data)
	{
		res = data.split('$');
		return this.checksum(res[0]) == res[1];
	},
	group: {
		p: bignum('177033325031863246295252620826138885780957322922066579920044128228185348887507084081440138453150019512773372306025463585789538483624405299436424597308170735408010229987919466024437150051872279268904279514287048719661141414496155305574442518410806024093822584284794312437055497350867776619583789215990493756683'),
		g: bignum(2)
	},
	inspect: function(data) // returns data with non printable chars written as \x..
	{
		let out = [ ];
		for (let i = 0 ; i < data.length ; i++)
			if (data[i] >= ' ' && data[i] < '~')
				out.push(data[i]);
			else
				out.push("\\x" + data.charCodeAt(i));
		return out.join('');
	},
	remove: function(data, c)
	{
		let re = new RegExp(c, "g");
		return data.replace(re, '');
	}
}

function buffer_to_bignum(buffer)
{
	var big = bignum();
	var hex = bignum(16);
	for (let i = buffer.length-1, k = 0; i >= 0; i--, k++)
		big = big.add((hex.pow(2*k)).multiply(buffer[i]));
	return big;
}

function bignum_to_buffer(bignum)
{
	var bytes = [ ];
	var hex = bignum.toString(16);
	var len = hex.length;
	if (len & 1 == 1)
	{
		hex = "0" + hex;
		len++;
	}
	for (let i = 0; i < len; i += 2)
		bytes.push(parseInt(hex.slice(i, i+2), 16));

	return Buffer(bytes);
}

function sha256(data)
{
	return crypto.createHash('sha256').update(data).digest();
}

var server = net.createServer(function(client) {
	console.log("[+] new connection from " + client.remoteAddress);
	client.on('error', (e) => { console.log("[-] An error has happened with the client from " + client.remoteAddress) ; client.end() });
	//client.on('close', (e) => { console.log("[-] Closed connection with client " + client.remoteAddress) ; client.end() });
	client.on('data', function(res)
	{
		try
		{
			if (client.dhe_initied)
			{
				let decrypted = security.decrypt(String(res), client.key);
				if (decrypted)
				{
					if (decrypted === "PING")
					{
						console.log("[+] PING from " + client.remoteAddress);
						client.write(security.encrypt("PONG", client.key) + "\n");
						console.log("[+] Already authentificated client");
						console.log(security.inspect(security.encrypt("a".repeat(500), client.key)));
					}
					else
					{
						// PROCESS DATA
						notifSender(decrypted);
						console.log("[+] Received data from " + client.remoteAddress + " : " + decrypted.toString());
						client.write(security.encrypt("ACK", client.key) + "\n");
					}
				}
				else
				{
					console.log("Data is erroned");
					
					client.write(security.encrypt("NAK", client.key) + "\n");
				}
				// The three way handshake
				
			}
			else
			{
				var server_key = buffer_to_bignum( crypto.randomBytes(128) );
				console.log('a = ' + server_key.toString(16));
				var client_key = buffer_to_bignum(Buffer(base64.decode(String(res).trim()), 'ascii'));
				console.log("g_b = " + client_key.toString(16));
				client.write(bignum_to_buffer(security.group.g.modPow(server_key, security.group.p)).toString('base64') + "\n");
				var secret = client_key.modPow(server_key, security.group.p);
				console.log('shared secret = ' + security.hexify(secret));
				console.log('Done diffie hellman');
				key = sha256(bignum_to_buffer(secret));
				client.key = key;
				console.log("secret = " + security.hexify(key));
				client.dhe_initied = true;
			}
		}
		catch (e)
		{
			console.log("In main : " + e);
			client.end();
		}
	});

});

server.on('error', (c) => { console.log("An error has happened : " + c.message) });
console.log('En attente d\'un client');
server.listen(1337, '0.0.0.0');
