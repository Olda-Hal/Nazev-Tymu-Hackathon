// Define this when using the App router API
'use client';

// !!Useful links at the bottom!!
// import {DeepChat as DeepChatCore} from 'deep-chat'; <- type
import dynamic from 'next/dynamic';
import axios from 'axios';
import styles from '../../styles/Chat.module.scss'

const instance = axios.create();

type Request = {
	instance: any;
}

instance.interceptors.request.use((config) => {
	console.log('Request: ', config);
	console.log('Request Data: ', config.data);
	return config;
}, (error) => {
	console.log('Request Error: ', error);
	return Promise.reject(error);
});

export default function HomePage() {
	const initialMessages = [
		{ role: 'ai', text: 'Vitej na Brno iD, jak vam mohu pomoci?' },
	];

	// need to import the component dynamically as it uses the 'window' property
	const DeepChat = dynamic(
		() => import('deep-chat-react').then((mod) => mod.DeepChat),
		{
			ssr: false,
		}
	);

	// demo/style/textInput are examples of passing an object directly into a property
	// initialMessages is an example of passing a state object into the property
	return (
		<div style={{ width: '100%', height: '700px', flex: 1 }}>
			<DeepChat
				request={{
					"url": "http://10.147.20.141:5118/AIPrompt/AskAI",
					"method": "POST",
				}}
				chatStyle={{
					display: 'block',
					width: 'inherit',
					height: 'inherit',
					borderRadius: '0',
					border: "0",
					backgroundColor: "#F1F3F5",
				}}
				messageStyles={{
					"default": {
						"shared": {
							"bubble": {
								"borderRadius": "0px",
								"padding": "20px"
							}
						},
						"user": { "bubble": { "backgroundColor": "#CF3231" } }
					}
				}}
				inputAreaStyle={{ backgroundColor: '#F1F3F5' }}
				textInput={{
					"styles": {
						"container": {
							"backroundColor": "red",
							"borderRadius": "0px",
							"bowShadow": "none",
							"width": "100%",
							"height": "70px"
						}
					}
				}}
				submitButtonStyles={{
					"position": "outside-right",
					"submit": {
						"container": {
							"default": {
								"backgroundColor": "#CF3231",
								"marginBottom": "0.1em",
								"marginLeft": "10px",
								"borderRadius": "0px",
								"flexShrink": "0",
							},
							"hover": { "backgroundColor": "#A62827" },
							"click": { "backgroundColor": "#962827" },
						},
						"text": {
							"content": "Odeslat",
							"styles": { "default": { "color": "white", "padding": "10px", "fontSize": "0.95em" } }
						}
					},
					"loading": { "container": { "default": { "backgroundColor": "#afafaf" } } },
					"stop": {
						"container": {
							"default": { "backgroundColor": "red" },
							"hover": { "backgroundColor": "red" },
							"click": { "backgroundColor": "red" }
						},
						"text": { "content": "Stop" }
					},
					"disabled": { "container": { "default": { backgroundColor: "#afafaf" } } },
				}}
				auxiliaryStyle="
					::-webkit-scrollbar-thumb {
					background-color: #5900ff;
					}"
				images={true}
				camera={true}
				mixedFiles={true}
				style={{ borderRadius: '0px' }}
				textInput={{ placeholder: { text: 'Doporuč mi sportovní akci.' } }}
				initialMessages={initialMessages}
			/>
		</div>
	)
}

// Info to get a reference for the component:
// https://github.com/OvidijusParsiunas/deep-chat/issues/59#issuecomment-1839483469

// Info to add types to a component reference:
// https://github.com/OvidijusParsiunas/deep-chat/issues/59#issuecomment-1839487740


