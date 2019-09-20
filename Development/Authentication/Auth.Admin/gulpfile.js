const gulp = require('gulp');
const concat = require('gulp-concat');
const uglify = require('gulp-uglify');
const sass = require('gulp-sass');
const minifyCSS = require('gulp-clean-css');
const del = require('del');

const distFolder = './wwwroot/dist/';
const jsFolder = `${distFolder}js/`;
const cssFolder = `${distFolder}css/`;

function processClean() {
    return del(`${distFolder}**`, { force: true });
}

function processScripts() {
    return gulp
        .src([
            './node_modules/jquery/dist/jquery.js',
            './node_modules/jquery-validation/dist/jquery.validate.js',
            './node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.js',
            './node_modules/popper.js/dist/umd/popper.js',
            './node_modules/bootstrap/dist/js/bootstrap.js',
            './node_modules/holderjs/holder.js',
            './node_modules/knockout/build/output/knockout-latest.js',
            './node_modules/toastr/toastr.js',
            './node_modules/moment/min/moment.min.js',
            './node_modules/tempusdominus-bootstrap-4/build/js/tempusdominus-bootstrap-4.js',
            './node_modules/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js',
            './Scripts/App/components/Menu.js',
            './Scripts/App/components/Picker.es5.js',
            './Scripts/App/components/Theme.js',
            './Scripts/App/helpers/FormMvcHelpers.js',
            './Scripts/App/helpers/jsontree.min.js',
            './Scripts/App/helpers/DateTimeHelpers.js',
            './Scripts/App/pages/ErrorsLog.es5.js',
            './Scripts/App/pages/Secrets.js',
            './Scripts/App/components/DatePicker.js'
        ])
        .pipe(concat('bundle.min.js'))
        .pipe(uglify())
        .pipe(gulp.dest(jsFolder));
}

function processFonts() {
    return gulp
        .src(['./node_modules/font-awesome/fonts/**', './node_modules/open-iconic/font/fonts/**'])
        .pipe(gulp.dest(`${distFolder}fonts/`));
}

function processSass() {
    return gulp
        .src('Styles/web.scss')
        .pipe(sass())
        .on('error', sass.logError)
        .pipe(gulp.dest(cssFolder));
}

function processSassMin() {
    return gulp
        .src('Styles/web.scss')
        .pipe(sass())
        .on('error', sass.logError)
        .pipe(minifyCSS())
        .pipe(concat('web.min.css'))
        .pipe(gulp.dest(cssFolder));
}

function processStyles() {
    return gulp
        .src([
            './node_modules/bootstrap/dist/css/bootstrap.css',
            './node_modules/toastr/build/toastr.css',
            './node_modules/open-iconic/font/css/open-iconic-bootstrap.css',
            './node_modules/font-awesome/css/font-awesome.css',
            './node_modules/tempusdominus-bootstrap-4/build/css/tempusdominus-bootstrap-4.css',
            './node_modules/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css',
            './Styles/controls/jsontree.css',
            './Styles/bootstrap_custom.min.css'
        ])
        .pipe(minifyCSS())
        .pipe(concat('bundle.min.css'))
        .pipe(gulp.dest(cssFolder));
}

const buildStyles = gulp.series(processStyles, processSass, processSassMin);
const build = gulp.parallel(buildStyles, processScripts);

gulp.task('clean', processClean);
gulp.task('styles', buildStyles);
gulp.task('sass', processSass);
gulp.task('sass:min', processSassMin);
gulp.task('fonts', processFonts);
gulp.task('scripts', processScripts);
gulp.task('build', build);
