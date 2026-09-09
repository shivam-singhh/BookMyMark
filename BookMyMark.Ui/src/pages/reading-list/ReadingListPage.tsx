import { useState } from 'react';
import {
  Alert,
  Badge,
  Button,
  Card,
  Container,
  Group,
  Image,
  SegmentedControl,
  SimpleGrid,
  Stack,
  Text,
  Title,
} from '@mantine/core';
import type { Book } from '../books/model';
import { READING_LIST_TITLE, READING_STATUS } from './constants';
import { useGetReadingListQuery, useRemoveFromReadingListMutation, useUpdateReadingStatusMutation } from './service';

interface ReadingListPageProps {
  books: Book[];
}

export function ReadingListPage({ books }: ReadingListPageProps) {
  const [shelf, setShelf] = useState<'All' | 'Reading' | 'Finished'>('All');
  const { data: items = [], isLoading } = useGetReadingListQuery(shelf === 'All' ? undefined : shelf);
  const [updateStatus] = useUpdateReadingStatusMutation();
  const [removeFromReadingList] = useRemoveFromReadingListMutation();

  const getBook = (bookId: number) => books.find((book) => book.id === bookId);

  const readingCount = items.filter((item) => item.status === READING_STATUS.reading).length;
  const finishedCount = items.filter((item) => item.status === READING_STATUS.finished).length;

  return (
    <Container size="xl" py="xl" className="page-container">
      <Stack gap={0} className="page-heading">
        <Text className="eyebrow">Your personal archive</Text>
        <Title className="page-title">{READING_LIST_TITLE}</Title>
        <Text className="page-subtitle">A record of the stories you are living with now, and the ones you have carried with you.</Text>
      </Stack>

      <Card className="shelf-summary" withBorder>
        <Group justify="space-between" align="center" wrap="wrap" gap="lg">
          <Group gap="xl">
            <div><Text className="stat-number">{items.length}</Text><Text className="stat-label">saved</Text></div>
            <div><Text className="stat-number">{readingCount}</Text><Text className="stat-label">in progress</Text></div>
            <div><Text className="stat-number">{finishedCount}</Text><Text className="stat-label">finished</Text></div>
          </Group>
          <SegmentedControl
            value={shelf}
            onChange={(value) => setShelf(value as 'All' | 'Reading' | 'Finished')}
            data={['All', 'Reading', 'Finished']}
            color="yellow"
          />
        </Group>
      </Card>

      {!isLoading && items.length === 0 && (
        <Alert className="empty-state" color="yellow" title="Your shelves are empty">
          Add a book from the catalog to start tracking your reading.
        </Alert>
      )}

      <SimpleGrid className="shelves-grid" cols={{ base: 1, sm: 2, md: 3 }}>
        {items.map((item) => {
          const book = getBook(item.bookId);
          if (!book) return null;

          const isFinished = item.status === READING_STATUS.finished;
          return (
            <Card key={item.id} withBorder radius="md" padding="lg" className="shelf-card">
              <Group wrap="nowrap" align="flex-start">
                <Image src={book.coverImageUrl} w={80} h={115} radius="xs" alt={book.title} />
                <Stack gap={4} style={{ flex: 1 }}>
                  <Title order={4} lineClamp={2}>{book.title}</Title>
                  <Text size="sm" c="dimmed">{book.author}</Text>
                  <Badge color={isFinished ? 'teal' : 'orange'} w="fit-content">
                    {item.status}
                  </Badge>
                  <Group gap="xs" mt="xs">
                    {!isFinished && (
                      <Button size="xs" variant="light" onClick={() => updateStatus({
                        id: item.id,
                        body: { status: READING_STATUS.finished },
                      })}>
                        Finish
                      </Button>
                    )}
                    <Button size="xs" color="red" variant="subtle" onClick={() => removeFromReadingList(item.id)}>
                      Remove
                    </Button>
                  </Group>
                </Stack>
              </Group>
            </Card>
          );
        })}
      </SimpleGrid>
    </Container>
  );
}
