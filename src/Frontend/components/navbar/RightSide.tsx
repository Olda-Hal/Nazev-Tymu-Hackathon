import React from 'react';
import Link from 'next/link';
import styles from '../../styles/navbar/RightSide.module.scss';
import FeatherIcon from 'feather-icons-react';

interface RightSideProps {
	strokeWidth?: number;
}

const RightSide: React.FC<RightSideProps> = ({ strokeWidth = 2 }) => {
	return (
		<ul className={styles['right-side']}>
			<li className={styles['nav-item']}>
				<Link href='#' className={styles['mobile']}>EN</Link>
				<Link href='#' className={styles['desktop']}>English</Link>
			</li>
			<li className={styles['nav-item']}>
				<Link href="http://localhost:3000/chat" passHref>
					<FeatherIcon icon='help-circle' className={styles['icon']} size="25" strokeWidth={strokeWidth} />
				</Link>
				<Link href="http://localhost:3000/chat" passHref className={styles['desktop']}>Chatbot</Link>
			</li>
			<li className={styles['nav-item']}>
				<Link href='#'>
					<FeatherIcon icon='user' className={styles['icon']} size="25" strokeWidth={strokeWidth} />
				</Link>
				<Link href='#' className={styles['desktop']}>Přihlásit se</Link>
			</li>
			<li className={styles['nav-item']}>
				<Link href='#'>
					<FeatherIcon icon='shopping-cart' className={styles['icon']} size="25" strokeWidth={strokeWidth} />
				</Link>
			</li>
			<li className={styles['nav-item']}>
				<Link href='#'>
					<FeatherIcon icon='align-left' className={`${styles['icon']} ${styles['icon-right']} ${styles['mobile']}`} size="19" strokeWidth={strokeWidth} />
				</Link>
			</li>
		</ul >
	);
}

export default RightSide;
