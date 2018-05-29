# vueze

> Learning vue

## Build Setup

``` bash
# install dependencies
npm install

# serve with hot reload at localhost:8080
npm run dev

# build for production with minification
npm run build

# build for production and view the bundle analyzer report and get the version to run in flask at localhost:5555
npm run build --report

```

For a detailed explanation on how things work, check out the [guide](http://vuejs-templates.github.io/webpack/) and [docs for vue-loader](http://vuejs.github.io/vue-loader).

In order for FLASK to pickup frontend changes, you need to compile them using npm run build. Build places the files in the static folder, used by Flask.