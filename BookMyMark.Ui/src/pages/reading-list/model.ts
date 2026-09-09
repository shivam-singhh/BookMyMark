import type { Book } from '../books/model';

export type ReadingStatus = 'Reading' | 'Finished';

export interface ReadingListItem {
  id: number;
  bookId: number;
  status: ReadingStatus;
  addedAt: string;
  finishedAt?: string;
  book?: Book;
}

export interface AddReadingListItemRequest {
  bookId: number;
}

export interface UpdateReadingStatusRequest {
  status: ReadingStatus;
}
