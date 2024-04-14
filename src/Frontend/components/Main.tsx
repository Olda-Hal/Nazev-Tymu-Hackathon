"use client"

import React from 'react'
import styles from '../styles/main/Main.module.scss'
import Square from './main/Square'

const Hero: React.FC = () => {
	return (
		<div className={styles['main']}>
			<div className={styles['main-wrapper']}>
				<ul className={styles['links-squares']}>
					<Square icon='airplay' spanText='MHD' />
					<Square icon='airplay' spanText='IDS JMK' />
					<Square icon='airplay' spanText='Odpady' />
					<Square icon='airplay' spanText='Rodičovské vouchery' />
					<Square icon='airplay' spanText='Knihovna' />
					<Square icon='airplay' spanText='Zoo' />
					<Square icon='airplay' spanText='Sport' />
					<Square icon='airplay' spanText='Hvězdárna' />
					<Square icon='airplay' spanText='Muzeum města Brna' />
					<Square icon='airplay' spanText='Městské hřbitovy' />
					<Square icon='airplay' spanText='Rezidentní parkování' />
					<Square icon='airplay' spanText='Veřejná zeleň města Brna' />
					<Square icon='airplay' spanText='Dáme na vás' />
				</ul>
			</div>
		</div>
	)
}

export default Hero;
