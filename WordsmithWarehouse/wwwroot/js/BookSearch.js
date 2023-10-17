new Vue({
    el: '#bookSearch',
    data() {
        return {
            tags: [],
            books: [],
            selectedTagsArray: [],
            filter: '',
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
            if (this.filter === '' && this.selectedTagsArray.length === 0) {
                // If no filter and no selected tags, return all books
                return this.books;
            }

            // Apply search filter
            const searchFilteredBooks = this.books.filter(book => {
                return book.title.toLowerCase().includes(this.filter.toLowerCase());
            });

            // Apply tag filter
            return searchFilteredBooks.filter(book => {
                if (this.selectedTagsArray.length === 0) {
                    return true; // No tags selected, so all books pass
                }
                const selectedTagsForBookArray = book.tagIds.split(',').map(Number);
                return selectedTagsForBookArray.some(tagId => this.selectedTagsArray.includes(tagId));
            });
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