import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { MantineProvider, createTheme } from '@mantine/core';
import { Provider } from 'react-redux';
import { store } from './app/store';
import { BooksPage } from './pages/books/BooksPage';
import '@mantine/core/styles.css';
import './style.css';

const theme = createTheme({
  primaryColor: 'yellow',
  fontFamily: 'Inter, Arial, sans-serif',
  headings: { fontFamily: 'Georgia, serif' },
  defaultRadius: 'sm',
});

createRoot(document.getElementById('app')!).render(
  <StrictMode>
    <Provider store={store}>
      <MantineProvider theme={theme} defaultColorScheme="dark">
        <BooksPage />
      </MantineProvider>
    </Provider>
  </StrictMode>,
);
