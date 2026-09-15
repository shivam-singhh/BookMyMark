import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { MantineProvider, createTheme } from '@mantine/core';
import { Provider } from 'react-redux';
import { store } from './app/store';
import { App } from './App';
import '@mantine/core/styles.css';
import './style.css';

const theme = createTheme({
  primaryColor: 'blue',
  fontFamily: "-apple-system, BlinkMacSystemFont, 'SF Pro Text', 'DM Sans', sans-serif",
  headings: { fontFamily: "-apple-system, BlinkMacSystemFont, 'SF Pro Display', 'DM Sans', sans-serif" },
  defaultRadius: 'md',
});

createRoot(document.getElementById('app')!).render(
  <StrictMode>
    <Provider store={store}>
      <MantineProvider theme={theme} defaultColorScheme="dark">
        <App />
      </MantineProvider>
    </Provider>
  </StrictMode>,
);
