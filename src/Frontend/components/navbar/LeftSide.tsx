import React from 'react';
import Link from 'next/link';
import styles from '../../styles/navbar/LeftSide.module.scss'
import FeatherIcon from 'feather-icons-react';

interface LeftSideProps {
	strokeWidth?: number;
}

const LeftSide: React.FC<LeftSideProps> = ({ strokeWidth = 2.5 }) => {
	return (
		<ul className={styles['left-side']}>
			<li className={styles['nav-item']}>
				<Link href="#">
					Produkty a služby
					<FeatherIcon icon='chevron-down' className={styles['icon']} size="12" strokeWidth={strokeWidth} />
				</Link>
			</li>
			<li className={styles['nav-item']}>
				<Link href="#">
					O Brno iD
				</Link>
			</li>
			<li className={styles['nav-item']}>
				<Link href="#">
					Časté dotazy
				</Link>
			</li>
		</ul>
	)
}

export default LeftSide;
