Vue.component('shelfCheckmark', {
    template: '#shelf-checkmark',
    data() {
        return {
            ticked: false,
        }
    },
})

new Vue({
    el: '#bookInfo',
    data() {
        return {
            shelves: [],
            bookId: bookId.value,
        }
    },
    created() {
        this.shelves = shelfData;
        console.log(this.shelves)
    },
    methods: {
        onAuthorClick(id) {
            console.log(id)
            window.location.assign(`/authors/details/${id}`);
        },
        getBookId(value) {
            $('#bookId').val(value);
        },

        loadShelfInfo(bId, sId) {
            let info = {
                bookId: bId,
                shelfId: sId,
            }

            return info;
        },

        sendToShelf() {

            var url = '/Shelves/AddToShelf';

            this.$http.post(url).then(function (response) {
                {
                    var res = response.body

                    if (res.result === 'success') {
                        console.log(res);
                        this.loadShelfInfo(this.bookId, )
                    } else {
                        console.log(res);
                    }

                }
            });
        },
        
        testMethod(value) {
            console.log(value);
        },
    },
    watch: {
        ticked(val, oldVal) {
            if (oldVal && val !== oldVAl) this.sendToShelf()
        }
    },
});

