// Define this when using the App router API
'use client';

import { useState } from 'react';
import dynamic from 'next/dynamic';

// Dynamic import
export default function FloatingChatbot() {
	const [isChatVisible, setChatVisible] = useState(false);

	const initialMessages = [
		{ role: 'ai', text: 'Vitej na Brno iD, jak vam mohu pomoci?' },
	];

	const DeepChat = dynamic(
		() => import('deep-chat-react').then((mod) => mod.DeepChat),
		{
			ssr: false,
		}
	);

	const messageStyles = {
		default: {
			shared: { bubble: { borderRadius: '0px' } },
			user: { bubble: { backgroundColor: '#CF3231' } },
		},
	};

	const inputAreaStyle = { backgroundColor: '#eeeeee', display: 'flex' };

	const textInput = {
		styles: {
			container: {
				backgroundColor: 'white',
				borderRadius: '0px',
				boxShadow: 'unset',
				width: '40%',
				marginRight: '-40px',
				maxWidth: '250px',
			},
		},
	};

	const submitButtonStyles = {
		position: 'outside-right',
		submit: {
			container: {
				default: {
					backgroundColor: '#CF3231',
					marginBottom: '0.1em',
					marginLeft: '10px',
					borderRadius: '0px',
				},
				hover: { backgroundColor: '#CF3231' },
				click: { backgroundColor: '#CF3231' },
			},
			text: {
				content: 'Send',
				styles: { default: { color: 'white', height: '2em', width: '3em', fontSize: '0.95em' } },
			},
		},
		loading: { container: { default: { backgroundColor: '#afafaf' } } },
		stop: {
			container: {
				default: { backgroundColor: '#919191' },
				hover: { backgroundColor: '#8c8c8c' },
				click: { backgroundColor: '#818181' },
			},
			text: { content: 'Stop' },
		},
		disabled: { container: { default: { backgroundColor: '#afafaf' } } },
	};

	const auxiliaryStyle = `
    ::-webkit-scrollbar-thumb {
        background-color: #CF3231;
    }
    `;

	const toggleChat = () => {
		setChatVisible(!isChatVisible);
	};

	return (
		<>
			<button
				onClick={toggleChat}
				style={{
					position: 'fixed',
					bottom: '1em',
					right: '1em',
					borderRadius: '50%',
					backgroundColor: '#CF3231',
					color: 'white',
					border: 'none',
					width: '60px',
					height: '60px',
					cursor: 'pointer',
					fontSize: '24px',
					display: 'flex',
					alignItems: 'center',
					justifyContent: 'center',
					zIndex: 1001,
				}}
			>
				{isChatVisible ? 'X' : '?'}
			</button>
			{isChatVisible && (
				<div style={{
					position: 'fixed',
					bottom: '80px',
					right: '20px',
					zIndex: 1000,
					width: '380px',
					height: '380px'
				}}>
					<DeepChat
						request={{
							"url": "http://10.147.20.141:5118/AIPrompt/AskAI",
							"method": "POST",
						}}
						style={{
							borderRadius: '0px',
							boxShadow: '0px 4px 8px rgba(0, 0, 0, 0.25)',
							position: 'relative',
						}}
						messageStyles={messageStyles}
						inputAreaStyle={inputAreaStyle}
						textInput={{ placeholder: { text: 'Doporuč mi sportovní akci.' } }}
						submitButtonStyles={{
							...submitButtonStyles,
							position: 'outside-right',
						}}
						auxiliaryStyle={auxiliaryStyle}
						initialMessages={initialMessages}
						stream={true}
					/>
				</div>
			)}
		</>
	);
}
