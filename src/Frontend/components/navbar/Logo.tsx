import React, { useState, useEffect } from 'react';
import styles from '../../styles/navbar/Logo.module.scss';

const LOGOS = [
	{ src: 'https://www.brnoid.cz/themes/brnoid/img/logo-brno-id.svg', alt: 'Logo 1' },
	{ src: 'https://www.brnoid.cz/themes/brnoid/img/logo-brno.svg', alt: 'Logo 2' },
];

const Logo: React.FC = () => {
	const [logoIndex, setLogoIndex] = useState(0);
	const [showNextLogo, setShowNextLogo] = useState(false);

	useEffect(() => {
		const interval = setInterval(() => {
			setShowNextLogo(true);
			setTimeout(() => {
				setLogoIndex((logoIndex + 1) % LOGOS.length);
				setShowNextLogo(false);
			}, 500);
		}, 8200);

		return () => clearInterval(interval);
	}, [logoIndex]);

	return (
		<div className={styles.logoContainer}>
			{LOGOS.map((logo, index) => (
				<img
					key={index}
					src={logo.src}
					alt={logo.alt}
					className={`${styles.navbarLogo} ${styles.logo} ${index === logoIndex && !showNextLogo ? '' : styles.hidden}`} />
			))}
		</div>
	);
};

export default Logo;
