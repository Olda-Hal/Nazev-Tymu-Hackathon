import React, { useState, useEffect } from 'react';
import { imageData as originalImageData } from '../../data/imageData';
import styles from '../../styles/hero/ImageSlider.module.scss';

const ImageSlider: React.FC = () => {
	const imageData = [...originalImageData, originalImageData[0]]; // Add the first image at the end
	const [currentImageIndex, setCurrentImageIndex] = useState(0);

	useEffect(() => {
		const timer = setInterval(() => {
			setCurrentImageIndex((prevIndex) => (prevIndex + 1) % imageData.length);
		}, 5000);
		return () => clearInterval(timer);
	}, []);

	const percentagePerImage = 100 / imageData.length;
	const offset = percentagePerImage * currentImageIndex;

	// Generate keyframes
	let keyframes = '';
	for (let i = 0; i < imageData.length; i++) {
		const startPercentage = i * percentagePerImage;
		const endPercentage = (i + 1) * percentagePerImage - 12; // Subtract 13% to pause for 8 seconds
		const pausePercentage = endPercentage + 12; // Add 13% to pause for 8 seconds
		keyframes += `
      ${startPercentage}% { transform: translateX(-${i * 100}%); }
      ${endPercentage}% { transform: translateX(-${i * 100}%); }
      ${pausePercentage}%, ${pausePercentage + percentagePerImage}% { transform: translateX(-${(i + 1) * 100}%); }
    `;
	}

	return (
		<div className={styles.slider}>
			<style>
				{`
          @keyframes slide {
            ${keyframes}
          }
        `}
			</style>
			<div className={styles.imageRow} style={{ animation: 'slide 200s infinite' }}>
				{imageData.map((image, index) => (
					<img
						key={index}
						src={image.src}
						alt={image.alt}
						className={styles.image}
					/>
				))}
			</div>
		</div>
	);
};

export default ImageSlider;
