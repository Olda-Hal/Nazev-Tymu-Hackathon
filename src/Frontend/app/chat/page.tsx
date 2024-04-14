import React from 'react';
import Navbar from '../../components/Navbar';
import Chatbot from '../../components/chatbot/Chatbot';
// import Footer from '../components/Footer';
import '../../styles/Chat.scss'

export default function HomePage() {
	return (
		<div className='main'>
			<div>
				<Navbar />
				<Chatbot />
				{/* <Footer /> */}
			</div>
		</div>
	);
}
