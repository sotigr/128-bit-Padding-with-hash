var Encoding = new function () {
    this.ASCII = new function () {
        this.GetBytes = function (msg) {
            var res = new Array();
            for (var i = 0; i < msg.length; i++) {
                res[i] = msg.charCodeAt(i);
            }
            return res;
        };
        this.GetString = function (array) {
            var result = "";
            for (var i = 0; i < array.length; i++) {
                result += String.fromCharCode(parseInt(array[i], 2));
            }
            return result;
        };
    };
};
Array.prototype.contains = function (obj) {
    var i = this.length;
    while (i--) {
        if (this[i] === obj) {
            return true;
        }
    }
    return false;
}
var sArray = new function () {
    this.Copy = function (src, srcOffset, dst, dstOffset, length) {

        src = src.subarray || src.slice ? src : src.buffer;
        dst = dst.subarray || dst.slice ? dst : dst.buffer;

        src = srcOffset ? src.subarray ?
            src.subarray(srcOffset, length && srcOffset + length) :
            src.slice(srcOffset, length && srcOffset + length) : src;

        if (dst.set) {
            dst.set(src, dstOffset);
        } else {
            for (var i = 0; i < src.length; i++) {
                dst[i + dstOffset] = src[i];
            }
        }

        return dst;
    };
};
function randomIntInc(low, high) {
    return Math.floor(Math.random() * (high - low + 1) + low);
}
var Random = new function () {
    this.NextBytes = function (len) {
        var numbers = new Array(len);
        for (var i = 0; i < numbers.length; i++) {
            numbers[i] = randomIntInc(1, 255)
        }
        return numbers;
    };
};


function PrintArray(array) {
    var msg = "";
    for (var i = 0; i < array.length; i++) {
        msg += array[i] + "\n";
    }
    alert(msg);
};


function CreateMask(text_length, masklength) {
    var existing = new Array();
    var arr = new Array(masklength);
    for (var i = 0; i < arr.Length; i++) {
        arr[i] = -1;
    }
    var cn = 0;
    while (cn < text_length) {
        var rand = randomIntInc(0, masklength);
        if (!existing.contains(rand)) {
            existing.push(rand);
            arr[rand] = 0;
            cn += 1;
        }
    }
    return arr;
}

function Pad(message, length, hash128) {
    var mLen = message.length;
    if (mLen > length || length > 64) {
        return null;
    }

    var hash = Encoding.ASCII.GetBytes(hash128);

    var mask = CreateMask(mLen, length);

    var padded = new Array(length);
    var msgcn = 0;
    for (var i = 0; i < length; i++) {
        if (mask[i] == 0) {
            padded[i] = message[msgcn];
            msgcn++;
        }
        else
            padded[i] = hash[i];
    }
    return padded;
}
function UnPad(message, hash128) {
    var mLen = message.length;
    var hash = Encoding.ASCII.GetBytes(hash128);
    var unpdaded = new Array();

    for (var i = 0; i < mLen; i++) {
        if (message[i] != hash[i])
            unpdaded.push(message[i]);
    }
    return unpdaded;
}

var message = Encoding.ASCII.GetBytes("this is a message");
PrintArray(message);

var randhash = sha256("safsdggadsad");
var padded = Pad(message, 64, randhash);
PrintArray(padded);
PrintArray(UnPad(padded, randhash));

