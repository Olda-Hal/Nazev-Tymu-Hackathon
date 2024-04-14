import React from 'react';
import Link from 'next/link';
import styles from '../../styles/main/Square.module.scss'
import FeatherIcon from 'feather-icons-react';

interface SquareProps {
	strokeWidth?: number;
	icon: string;
	spanText: string;
}

const Square: React.FC<SquareProps> = ({ strokeWidth = 2.5, icon, spanText }) => {
	return (
		<li>
			<a className={styles['btn']} href="#">
				<div className={styles['icon-text-container']}>
					<FeatherIcon icon={icon} className={styles['icon']} size="25" strokeWidth={strokeWidth} />
					<span>{spanText}</span>
				</div>
			</a>
		</li>
	)
}

export default Square;
