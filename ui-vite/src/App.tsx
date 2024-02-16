import './App.css'

import dayjs from 'dayjs'
import duration from 'dayjs/plugin/duration';
import relativeTime from 'dayjs/plugin/relativeTime';
import { RouterProvider, createBrowserRouter } from 'react-router-dom'
import Dashboard from './components/dashboard/page'

import 'bootstrap/dist/css/bootstrap.min.css';

dayjs.extend(duration);
dayjs.extend(relativeTime);

function App() {

  const router = createBrowserRouter([
    {
      path: '',
      Component: Dashboard
    }
  ])

  return (
    <RouterProvider router={router} fallbackElement={<p>Loading...</p>} />
  )
}

export default App
