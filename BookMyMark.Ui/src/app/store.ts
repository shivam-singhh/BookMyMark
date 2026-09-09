import { configureStore } from '@reduxjs/toolkit';
import { booksApi } from '../pages/books/service';
import { readingListApi } from '../pages/reading-list/service';

export const store = configureStore({
  reducer: {
    [booksApi.reducerPath]: booksApi.reducer,
    [readingListApi.reducerPath]: readingListApi.reducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(booksApi.middleware, readingListApi.middleware),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
