import React from 'react';
import Navbar from '../components/Navbar';
import Hero from '../components/Hero';
import Main from '../components/Main';
import FloatingChatbot from '../components/chatbot/FloatingChatbot'
// import Footer from '../components/Footer';
import '../styles/globals.scss';

export default function HomePage() {
	return (
		<div>
			<div>
				<Navbar />
				<Hero />
				<Main />
				<FloatingChatbot />
				{/* <Footer /> */}
			</div>
		</div>
	);
}
