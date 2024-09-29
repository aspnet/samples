const path = require('path');
const CopyPlugin = require('copy-webpack-plugin');

function getStyleUse(bundleFilename) {
    return [
        {
            loader: 'file-loader',
            options: {
                name: bundleFilename,
            },
        },
        { loader: 'extract-loader' },
        { loader: 'css-loader' },
        { loader: 'sass-loader',
            options: {
                sassOptions: {
                    includePaths: ['./node_modules']
                },
                implementation: require('dart-sass')
            }
        },
    ];
}

module.exports = [
    {
        entry: { 'styles': './app/site.scss' },
        output: {
            // This is necessary for webpack to compile, but we never reference this js file.
            path: path.resolve(__dirname, 'wwwroot/dist'),
            filename: 'style.bundle.js',
            publicPath: '/'
        },
        module: {
            rules: [{
                test: [/\.scss$/, /\.css$/],
                use: getStyleUse('app.bundle.css')
            }]
        },
        plugins: [
            new CopyPlugin([
                { from: 'app/images', to: 'images' },
                { from: 'app/favicon.ico', to: 'favicon.ico' },
            ]),
        ]
    },
    {
        entry: { 'main': './app/app.js' },
        output: {
            path: path.resolve(__dirname, 'wwwroot/dist'),
            filename: 'app.bundle.js',
            publicPath: '/'
        },
        module: {
            rules: [
                {
                    test: /\.js$/,
                    loader: 'babel-loader',
                    query: { presets: ['env'] }
                },
                {
                    test: /\.jpe?g$|\.ico$/,
                    loader: 'file-loader',
                    options: {
                        name: '[name].[ext]'
                    }
                }
            ]
        },
    }
];