import { createBrowserRouter, RouterProvider } from 'react-router'

import { Layout } from '@/components/layout/layout'
import { Home } from '@/pages/home'

const router = createBrowserRouter([
  {
    path: '/',
    element: <Layout />,
    children: [{ index: true, element: <Home /> }],
  },
])

function App() {
  return <RouterProvider router={router} />
}

export default App
