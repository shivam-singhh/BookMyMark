import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { Book } from './model';
import { API_BASE_URL } from './constants';

export const booksApi = createApi({
  reducerPath: 'booksApi',
  baseQuery: fetchBaseQuery({ baseUrl: API_BASE_URL }),
  endpoints: (builder) => ({
    getBooks: builder.query<Book[], void>({
      query: () => '/books',
    }),
    getBookById: builder.query<Book, number>({
      query: (id) => `/books/${id}`,
    }),
  }),
});

export const { useGetBooksQuery, useGetBookByIdQuery } = booksApi;
