"use client"

import React from 'react';
import styles from '../../../styles/hero/UserCount.module.scss'

const UserCount: React.FC = () => {
	return (
		<div className={styles['user-count']}>
			<div className={styles['the-number']}>
				<i className={`${styles['nevim']} ${styles['v-middle']}`}>
				</i>
				<span className={styles['v-middle']}>570 tisíc</span>
			</div>
			<div className={styles['the-label']}>
				<div className={styles['primary']}>
					Registrovaných uživatelů
				</div>
				<div className={styles['secondary']}>
					máme už přes 570 tisíc <br></br> registrovaných uživatelů
				</div>
			</div>
		</div>
	);
};

export default UserCount;
