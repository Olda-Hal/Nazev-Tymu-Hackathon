"use client"

import React from "react"
import styles from '../../../styles/hero/PromoText.module.scss'

const PromoText: React.FC = () => {
	return (
		<div className={styles['promo']}>
			<h1 className={styles['promo-headline']}>Řešte věci online <span className={styles['inline-block']}> s portálem <strong className={styles['text-bold-red']}> Brno iD </strong></span></h1>
			<p className={styles['text-gray']}>Z jednoho jediného elektronického účtu vyřídíte stále více věcí, kvůli kterým jste dřív museli běhat na přepážky.</p>
			<a className={styles['btn']} href="#">Zjistit více</a>
		</div>
	)
}

export default PromoText;
