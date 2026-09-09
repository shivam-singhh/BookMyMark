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
import { useGetBooksQuery } from './service';
import { useAddToReadingListMutation } from '../reading-list/service';
import { ReadingListPage } from '../reading-list/ReadingListPage';

export function BooksPage() {
  const { data: books, isLoading, isError } = useGetBooksQuery();
  const [addToReadingList, { isLoading: isAdding }] = useAddToReadingListMutation();

  return (
    <Container size="xl" py="xl">
      <header className="site-header">
        <Text className="brand">book<span>mymark</span></Text>
        <Group gap="xs">
          <Badge color="yellow" variant="filled">CATALOG</Badge>
          <Text size="sm" c="dimmed">A personal library</Text>
        </Group>
      </header>
      <Stack gap="xs" mb="xl">
        <Text c="yellow" fw={700} tt="uppercase" size="sm">Browse the collection</Text>
        <Title className="hero-title">{BOOKS_PAGE_TITLE}</Title>
        <Text c="dimmed">Browse the catalog and keep track of what you are reading.</Text>
      </Stack>

      {isError && (
        <Alert color="red" title="Unable to load books" mb="xl">
          Make sure the ASP.NET API is running on http://localhost:5087.
        </Alert>
      )}

      {isLoading && (
        <SimpleGrid cols={{ base: 1, sm: 2, md: 3, lg: 5 }}>
          {Array.from({ length: 5 }, (_, index) => (
            <Card key={index} withBorder padding="md">
              <Skeleton height={240} mb="md" />
              <Skeleton height={20} mb="sm" />
              <Skeleton height={16} width="70%" />
            </Card>
          ))}
        </SimpleGrid>
      )}

      <Grid mb={80}>
        {books?.map((book) => (
          <Grid.Col key={book.id} span={{ base: 12, sm: 6, md: 4, lg: 3 }}>
            <Card withBorder radius="sm" padding="sm" h="100%" className="book-card">
              <Card.Section>
                <Image src={book.coverImageUrl} height={260} alt={book.title} fallbackSrc="" />
              </Card.Section>
              <Stack gap="xs" mt="md">
                <Group justify="space-between" align="flex-start">
                  <Title order={3} lineClamp={2}>
                    {book.title}
                  </Title>
                  <Badge variant="light">{book.publishedYear}</Badge>
                </Group>
                <Text c="dimmed">by {book.author}</Text>
                <Group justify="space-between" mt="xs">
                  {book.genre && <Badge color="gray" variant="light">{book.genre}</Badge>}
                  <Button
                    size="xs"
                    color="yellow"
                    variant="light"
                    loading={isAdding}
                    onClick={() => addToReadingList({ bookId: book.id })}
                  >
                    + Add to shelf
                  </Button>
                </Group>
              </Stack>
            </Card>
          </Grid.Col>
        ))}
      </Grid>
      {books && <ReadingListPage books={books} />}
    </Container>
  );
}
