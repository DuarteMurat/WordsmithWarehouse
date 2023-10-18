Vue.component('libraryInput', {
    template: '#library-input',
    data() {
        return {
            stock: 0,
        }
    },
})
new Vue({
    el: '#bookStock',
    data() {
        return {
            libraries: [],
        }
    },
    created() {
        this.libraries = libraryData;
        console.log(this.libraries);
    },
})