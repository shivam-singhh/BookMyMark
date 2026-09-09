import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { API_BASE_URL } from '../books/constants';
import type {
  AddReadingListItemRequest,
  ReadingListItem,
  ReadingStatus,
  UpdateReadingStatusRequest,
} from './model';

export const readingListApi = createApi({
  reducerPath: 'readingListApi',
  baseQuery: fetchBaseQuery({ baseUrl: API_BASE_URL }),
  tagTypes: ['ReadingList'],
  endpoints: (builder) => ({
    getReadingList: builder.query<ReadingListItem[], ReadingStatus | undefined>({
      query: (status) => ({
        url: '/reading-list',
        params: status ? { status } : undefined,
      }),
      providesTags: ['ReadingList'],
    }),
    addToReadingList: builder.mutation<ReadingListItem, AddReadingListItemRequest>({
      query: (body) => ({ url: '/reading-list', method: 'POST', body }),
      invalidatesTags: ['ReadingList'],
    }),
    updateReadingStatus: builder.mutation<
      ReadingListItem,
      { id: number; body: UpdateReadingStatusRequest }
    >({
      query: ({ id, body }) => ({ url: `/reading-list/${id}/status`, method: 'PUT', body }),
      invalidatesTags: ['ReadingList'],
    }),
    removeFromReadingList: builder.mutation<void, number>({
      query: (id) => ({ url: `/reading-list/${id}`, method: 'DELETE' }),
      invalidatesTags: ['ReadingList'],
    }),
  }),
});

export const {
  useGetReadingListQuery,
  useAddToReadingListMutation,
  useUpdateReadingStatusMutation,
  useRemoveFromReadingListMutation,
} = readingListApi;
