const express = require('express');

const app = express();

app.use('/', express.static('.'));

app.listen(9000, () => console.log('Listening port 9000...'));