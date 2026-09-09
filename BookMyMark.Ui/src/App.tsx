import { useState } from 'react';
import { Badge, Button, Divider, Group, Stack, Text } from '@mantine/core';
import { useGetBooksQuery } from './pages/books/service';
import { BooksPage } from './pages/books/BooksPage';
import { ReadingListPage } from './pages/reading-list/ReadingListPage';

type View = 'discover' | 'shelves';

export function App() {
  const [view, setView] = useState<View>('discover');
  const { data: books = [], isLoading, isError } = useGetBooksQuery();

  return (
    <div className="app-shell">
      <aside className="sidebar">
        <div>
          <Text className="brand">book<span>mymark</span></Text>
          <Text className="sidebar-tagline">A quiet place for your reading life.</Text>
        </div>

        <nav className="main-nav" aria-label="Main navigation">
          <Button
            className={`nav-item ${view === 'discover' ? 'is-active' : ''}`}
            variant="subtle"
            onClick={() => setView('discover')}
          >
            <span className="nav-icon">⌕</span>
            <span>Discover</span>
          </Button>
          <Button
            className={`nav-item ${view === 'shelves' ? 'is-active' : ''}`}
            variant="subtle"
            onClick={() => setView('shelves')}
          >
            <span className="nav-icon">▤</span>
            <span>My shelves</span>
            <Badge size="sm" variant="light" color="yellow">Library</Badge>
          </Button>
        </nav>

        <Stack gap="sm" className="sidebar-footer">
          <Divider color="dark.4" />
          <Group gap="xs">
            <div className="avatar">M</div>
            <div>
              <Text size="sm" fw={700}>My library</Text>
              <Text size="xs" c="dimmed">Personal collection</Text>
            </div>
          </Group>
        </Stack>
      </aside>

      <main className="main-content">
        <header className="mobile-header">
          <Text className="brand">book<span>mymark</span></Text>
          <Badge color="yellow" variant="light">LIBRARY</Badge>
        </header>
        {view === 'discover' ? (
          <BooksPage books={books} isLoading={isLoading} isError={isError} />
        ) : (
          <ReadingListPage books={books} />
        )}
      </main>
      <nav className="mobile-nav" aria-label="Mobile navigation">
        <Button
          variant="subtle"
          className={view === 'discover' ? 'mobile-nav-item is-active' : 'mobile-nav-item'}
          onClick={() => setView('discover')}
        >
          <span className="nav-icon">⌕</span>
          <span>Discover</span>
        </Button>
        <Button
          variant="subtle"
          className={view === 'shelves' ? 'mobile-nav-item is-active' : 'mobile-nav-item'}
          onClick={() => setView('shelves')}
        >
          <span className="nav-icon">▤</span>
          <span>My shelves</span>
        </Button>
      </nav>
    </div>
  );
}
