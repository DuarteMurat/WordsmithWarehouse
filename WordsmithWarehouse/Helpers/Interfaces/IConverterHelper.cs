﻿using ClassLibrary.Entities;
using WordsmithWarehouse.Models;

namespace WordsmithWarehouse.Helpers.Interfaces
{
    public interface IConverterHelper
    {
        Book ConvertToBook(BookViewModel model, string path, bool isNew);

        BookViewModel ConvertToBookViewModel(Book book);
    }
}