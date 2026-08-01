import { Link, Outlet } from 'react-router'

import { ThemeToggle } from '@/components/theme-toggle'

export function Layout() {
  return (
    <div className="flex min-h-svh flex-col">
      <header className="flex items-center justify-between border-b border-border px-6 py-4">
        <Link to="/" className="text-lg font-semibold">
          Certification Assessment Platform
        </Link>
        <ThemeToggle />
      </header>
      <main className="flex-1 px-6 py-8">
        <Outlet />
      </main>
    </div>
  )
}
