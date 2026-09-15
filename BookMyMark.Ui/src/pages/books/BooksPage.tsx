import {
  Alert,
  Badge,
  Button,
  Card,
  Container,
  Grid,
  Group,
  Image,
  SimpleGrid,
  Skeleton,
  Stack,
  Text,
  Title,
} from '@mantine/core';
import { BOOKS_PAGE_TITLE } from './constants';
import {
  useAddToReadingListMutation,
  useGetReadingListQuery,
} from '../reading-list/service';
import type { Book } from './model';

interface BooksPageProps {
  books: Book[];
  isLoading: boolean;
  isError: boolean;
}

export function BooksPage({ books, isLoading, isError }: BooksPageProps) {
  const { data: readingList = [] } = useGetReadingListQuery(undefined);
  const [addToReadingList, { isLoading: isAdding, isSuccess, isError: isAddError }] =
    useAddToReadingListMutation();
  const savedBookIds = new Set(readingList.map((item) => item.bookId));

  return (
    <Container size="xl" py="xl" className="page-container">
      <Stack gap={4} mb="xl" className="page-heading">
        <Text className="eyebrow">Good evening</Text>
        <Title className="page-title">{BOOKS_PAGE_TITLE}</Title>
        <Text className="page-subtitle">Find your next great read and save it to your shelves.</Text>
      </Stack>

      {isError && (
        <Alert color="red" title="Unable to load books" mb="xl">
          Make sure the ASP.NET API is running on http://localhost:5087.
        </Alert>
      )}

      {isSuccess && (
        <Alert color="teal" title="Added to your shelf" mb="xl">
          Open <strong>My shelves</strong> to see your reading list.
        </Alert>
      )}

      {isAddError && (
        <Alert color="red" title="Could not add this book" mb="xl">
          The request reached the API, but the book was not saved. Please try again.
        </Alert>
      )}

      {isLoading && (
        <SimpleGrid className="book-grid" cols={{ base: 2, xs: 3, sm: 3, md: 4, lg: 5 }}>
          {Array.from({ length: 5 }, (_, index) => (
            <Card key={index} withBorder padding="sm" className="book-card">
              <Skeleton height={240} mb="md" />
              <Skeleton height={20} mb="sm" />
              <Skeleton height={16} width="70%" />
            </Card>
          ))}
        </SimpleGrid>
      )}

      <Grid className="book-grid" mb={80}>
        {books?.map((book) => (
          <Grid.Col key={book.id} span={{ base: 6, sm: 4, md: 3, lg: 3 }}>
            <Card withBorder radius="lg" padding="sm" h="100%" className="book-card">
              <Card.Section>
                <Image src={book.coverImageUrl} height={260} alt={book.title} fallbackSrc="" />
              </Card.Section>
              <Stack gap="xs" mt="md">
                <Group justify="space-between" align="flex-start" gap="xs">
                  <Title order={3} lineClamp={2} className="book-title">
                    {book.title}
                  </Title>
                </Group>
                <Text c="dimmed" size="sm" lineClamp={1}>by {book.author}</Text>
                <Group justify="space-between" mt="xs" gap="xs">
                  {book.genre && <Badge color="gray" variant="light" size="sm">{book.genre}</Badge>}
                  <Button
                    size="xs"
                    color="blue"
                    variant={savedBookIds.has(book.id) ? 'light' : 'filled'}
                    loading={isAdding && !savedBookIds.has(book.id)}
                    disabled={savedBookIds.has(book.id)}
                    onClick={() => addToReadingList({ bookId: book.id })}
                  >
                    {savedBookIds.has(book.id) ? 'Saved' : 'Read'}
                  </Button>
                </Group>
              </Stack>
            </Card>
          </Grid.Col>
        ))}
      </Grid>
    </Container>
  );
}
