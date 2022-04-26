const express = require("express");
const app = express();
app.use(express.static("../ui/"));
const path = require("path");
app.get("*", (req, res) => {
    res.sendFile(path.resolve(__dirname, "..", "ui", "index.html"));
});

const PORT = 5001;
console.log("server started on port:", PORT);
app.listen(PORT);
