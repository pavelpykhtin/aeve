const path = require('path');
module.exports = {
    entry: './app/index.jsx',
    output: {
        filename: 'bundle.js',
        path: path.resolve(__dirname, 'dist')
    },
    module: {
        rules: [{
            test: /\.jsx$/,
            include: [path.resolve(__dirname, 'app')],
            exclude: [
                path.resolve(__dirname, 'node_modules'),
                path.resolve(__dirname, 'bower_components')
            ],
            loader: 'babel-loader',
            query: {
                presets: ['react', 'env'],
                plugins: [
                    ['transform-object-rest-spread',
                        {
                            "useBuiltIns": false
                        }
                    ]
                ],
            }
        }, {
            test: /\.js$/,
            include: [path.resolve(__dirname, 'app')],
            exclude: [
                path.resolve(__dirname, 'node_modules'),
                path.resolve(__dirname, 'bower_components')
            ],
            loader: 'babel-loader',
            query: {
                presets: ['env']
            }
        }, {
            test: /\.css$/,
            loaders: [
                'style-loader', {
                    loader: 'css-loader',
                    options: {
                        modules: true
                    }
                }
            ]
        }, {
            test: /\.less$/,
            loaders: [
                'style-loader', {
                    loader: 'css-loader'
                },
                'less-loader'
            ]
        }]
    },
    resolve: {
        extensions: ['.json', '.js', '.jsx', '.css', '.less']
    },
    devtool: 'source-map'
};