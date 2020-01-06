//#region Imports
var CSON = require("cson"),
    path = require("path"),
    fs = require("fs");
//#endregion

//#region Process
var processCSON2JSON = function (data, sourceFileName, targetFileName, writer) {
    data = CSON.parseCSONString(data);

    if (typeof data === 'Error') {
        var error = "CSON: Invalid CSON content found";

        writer.write(JSON.stringify({
            Success: false,
            SourceFileName: sourceFileName,
            TargetFileName: targetFileName,
            Remarks: error,
            Details: error,
            Errors: [{
                Message: error,
                FileName: sourceFileName
            }]
        }));
        writer.end();
        return;
    }

    writer.write(JSON.stringify({
        Success: true,
        SourceFileName: sourceFileName,
        TargetFileName: targetFileName,
        Remarks: "Successful!",
        Content: CSON.createJSONString(data)
    }));
    writer.end();
};

var processJSON2CSON = function (data, sourceFileName, targetFileName, writer) {
    data = CSON.parseJSONString(data);

    if (typeof data === 'Error') {
        var err = "CSON: Invalid JSON content found";

        writer.write(JSON.stringify({
            Success: false,
            SourceFileName: sourceFileName,
            TargetFileName: targetFileName,
            Remarks: err,
            Details: err,
            Errors: [{
                Message: err,
                FileName: sourceFileName
            }]
        }));
        writer.end();
        return;
    }

    writer.write(JSON.stringify({
        Success: true,
        SourceFileName: sourceFileName,
        TargetFileName: targetFileName,
        Remarks: "Successful!",
        Content: CSON.createCSONString(data)
    }));
    writer.end();
};
//#endregion

//#region Handler
var handleCSON = function (writer, params) {
    if (!fs.existsSync(params.sourceFileName)) {
        writer.write(JSON.stringify({
            Success: false,
            SourceFileName: params.sourceFileName,
            TargetFileName: params.targetFileName,
            Remarks: "CSON: Input file not found!",
            Errors: [{
                Message: "CSON: Input file not found!",
                FileName: params.sourceFileName
            }]
        }));
        writer.end();
        return;
    }

    fs.readFile(params.sourceFileName, 'utf8', function (err, data) {
        if (err) {
            writer.write(JSON.stringify({
                Success: false,
                SourceFileName: params.sourceFileName,
                TargetFileName: params.targetFileName,
                Remarks: "CSON: Error reading input file.",
                Details: err,
                Errors: [{
                    Message: "CSON: " + err,
                    FileName: params.sourceFileName
                }]
            }));
            writer.end();
            return;
        }

        if (path.extname(params.sourceFileName).toUpperCase() === ".JSON") {
            processJSON2CSON(data, params.sourceFileName, params.targetFileName, writer);
        } else {
            processCSON2JSON(data, params.sourceFileName, params.targetFileName, writer);
        }
    });
};
//#endregion

//#region Exports
module.exports = handleCSON;
//#endregion
