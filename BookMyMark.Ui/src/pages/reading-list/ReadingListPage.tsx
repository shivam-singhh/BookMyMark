import { Alert, Badge, Button, Card, Group, Image, SimpleGrid, Stack, Text, Title } from '@mantine/core';
import type { Book } from '../books/model';
import { READING_LIST_TITLE, READING_STATUS } from './constants';
import { useGetReadingListQuery, useRemoveFromReadingListMutation, useUpdateReadingStatusMutation } from './service';

interface ReadingListPageProps {
  books: Book[];
}

export function ReadingListPage({ books }: ReadingListPageProps) {
  const { data: items = [], isLoading } = useGetReadingListQuery(undefined);
  const [updateStatus] = useUpdateReadingStatusMutation();
  const [removeFromReadingList] = useRemoveFromReadingListMutation();

  const getBook = (bookId: number) => books.find((book) => book.id === bookId);

  return (
    <Stack gap="lg">
      <Group justify="space-between" align="end">
        <div>
          <Text c="dimmed" size="sm" tt="uppercase" fw={700}>Your shelves</Text>
          <Title order={2}>{READING_LIST_TITLE}</Title>
        </div>
        <Badge size="lg" variant="light">{items.length} saved</Badge>
      </Group>

      {!isLoading && items.length === 0 && (
        <Alert color="yellow" title="Your shelves are empty">
          Add a book from the catalog to start tracking your reading.
        </Alert>
      )}

      <SimpleGrid cols={{ base: 1, sm: 2, md: 3 }}>
        {items.map((item) => {
          const book = getBook(item.bookId);
          if (!book) return null;

          const isFinished = item.status === READING_STATUS.finished;
          return (
            <Card key={item.id} withBorder radius="md" padding="sm" className="shelf-card">
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
    </Stack>
  );
}
