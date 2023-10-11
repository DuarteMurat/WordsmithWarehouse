new Vue({
    el: '#bookSearch',
    data() {
        return {
            tags: [],
            books: [],
            selectedTagsArray: []
        }
    },
    created() {
        this.tags = TagsJson;
        this.books = BooksJson;
        console.log(this.tags);
        console.log(this.books);
    },
    computed: {
        userFilters() {
            return this.tags.filter(t => !t.isAdmin)
        },
        filteredBooks() {
            if (this.selectedTagsArray.length === 0) return this.books

            return this.books.filter((book) => {
                const selectedTagsForBookArray = book.tagIds.split(',');
                return selectedTagsForBookArray.some(i => this.selectedTagsArray.includes(Number(i)))
            })
        },
        
    },
    methods: {
        onTagCheckboxClicked(tagId) {
            const checkedYN = event.target.checked;

            // add selectedTagsArray to the data side of things as selectedTagsArray: []
            if (checkedYN) this.selectedTagsArray.push(tagId)
            else {
                const indexOfTagId = this.selectedTagsArray.findIndex(tag => tag.id === tagId)
                this.selectedTagsArray.splice(indexOfTagId, 1)
            }
        },

        onBookClick(id) {
            console.log(id)
            window.location.assign(`/books/details/${id}`);
        },
        getBookImage(imagePath) {
            if (imagePath.charAt(0) === '~') {
                return imagePath.substring(1);
            }
            return imagePath
        },
    }
})