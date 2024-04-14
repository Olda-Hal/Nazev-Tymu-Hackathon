import { Open_Sans } from 'next/font/google'

export const metadata = {
	title: 'E-shop Brno iD',
	description: 'E-shop Brno iD',
}

const open_sans = Open_Sans({ subsets: ['latin'] })

export default function RootLayout({
	children,
}: {
	children: React.ReactNode
}) {
	return (
		<html lang="en">
			<body className={open_sans.className}>{children}</body>
		</html>
	)
}
