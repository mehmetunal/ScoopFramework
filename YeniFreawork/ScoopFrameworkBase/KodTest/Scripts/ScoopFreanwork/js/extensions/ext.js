var ScoopFreanworkJS = {
    Extensions: {
        format: function (value, fmt, culture) {
            culture = culture ? culture : navigator.language;
            if (fmt) {
                if ((value) instanceof Date && Object.prototype.toString.call(value) === "[object Date]") {
                    var options = { year: "numeric", month: "numeric", day: "numeric", hour: "numeric", minute: "numeric", second: "numeric", hour12: false };
                    return new Intl.DateTimeFormat(culture, options).format(value);
                } else if (typeof value === "number") {
                    return new Intl.NumberFormat(culture, { maximumSignificantDigits: fmt.replaceAll("n", "") }).format(value);
                }
            }
            return value !== undefined ? Intl.NumberFormat(culture).format(value) : "";
        },
        ParseDate: function (dateString) {
            //dd.mm.yyyy, or dd.mm.yy
            var dateArr = dateString.split(".");
            if (dateArr.length == 1) {
                return null;    //wrong format
            }
            //parse time after the year - separated by space
            var spacePos = dateArr[2].indexOf(" ");
            if (spacePos > 1) {
                var timeString = dateArr[2].substr(spacePos + 1);
                var timeArr = timeString.split(":");
                dateArr[2] = dateArr[2].substr(0, spacePos);
                if (timeArr.length == 2) {
                    //minutes only
                    return new Date(parseInt(dateArr[2]), parseInt(dateArr[1] - 1), parseInt(dateArr[0]), parseInt(timeArr[0]), parseInt(timeArr[1]));
                } else {
                    //including seconds
                    return new Date(parseInt(dateArr[2]), parseInt(dateArr[1] - 1), parseInt(dateArr[0]), parseInt(timeArr[0]), parseInt(timeArr[1]), parseInt(timeArr[2]))
                }
            } else {
                //gotcha at months - January is at 0, not 1 as one would expect
                return new Date(parseInt(dateArr[2]), parseInt(dateArr[1] - 1), parseInt(dateArr[0]));
            }
        }
    }
}

//İKİNCİ YAZIM ŞEKLİ BU 
//var ScoopFreanworkJS = () => ({
//    Extensions: () =>  ({
//        format: (value, fmt, culture) => {
//            culture = culture ? culture : navigator.language;
//            if (fmt) {
//                if ((value) instanceof Date && Object.prototype.toString.call(value) === '[object Date]') {
//                    var options = { year: 'numeric', month: 'numeric', day: 'numeric', hour: 'numeric', minute: 'numeric', second: 'numeric', hour12: false };
//                    return new Intl.DateTimeFormat(culture, options).format(value);
//                } else if (typeof value === "number") {
//                    return new Intl.NumberFormat(culture, { maximumSignificantDigits: fmt.replaceAll('n', '') }).format(value);
//                }
//            }
//            return value !== undefined ? Intl.NumberFormat(culture).format(value) : '';
//        }
//    })
//});

String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.split(search).join(replacement);
};

String.format = function (fmtstr) {
    var args = Array.prototype.slice.call(arguments, 1);
    return fmtstr.replace(/\{(\d+)\}/g, function (match, index) {
        return args[index];
    });
}

///console.log("I {} this is what {2} want and {} works for {2}!".format("hope","it","you"))
String.prototype.format = function () {
    var content = this;
    for (var i = 0; i < arguments.length; i++) {
        var target = '{' + i + '}';
        content = content.split(target).join(String(arguments[i]));
        content = content.replace("{}", String(arguments[i]));
    }
    return content;
}

//para formatı  =>   new Intl.NumberFormat('tr-TR', { style: 'currency', currency: 'TRY' }).format(123456.789)
/*
RegExp.prototype

var re = /n/g; 
var str = 'n3';
var newstr = re[Symbol.replace](str, '');


*/

/*
        lesson



        var func = x => x * x;                  
            // concise body syntax, implied "return"

        var func = (x, y) => { return x + y; }; 
            // with block body, explicit "return" needed


        var func = () => { foo: 1 };               
            // Calling func() returns undefined!

        var func = () => { foo: function() {} };   
            // SyntaxError: function statement requires a name


        var func = () => ({foo: 1});
        
        var func = ()=> 1; 
            // SyntaxError: expected expression, got '=>'




*/