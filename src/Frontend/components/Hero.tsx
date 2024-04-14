"use client"

import React from 'react'
import styles from '../styles/hero/Hero.module.scss'
import PromoText from './hero/leftside/PromoText'
import UserCount from './hero/leftside/UserCount'
import ImageSlider from './hero/ImageSlider'

const Hero: React.FC = () => {
	return (
		<div className={styles['hero-wrap']}>          
			<div className={styles['hero']}>          
				<div className={styles['left-side']}>          
					<PromoText />          
					<UserCount />          
				</div>          
				<div className={styles['right-side']}          >
					<ImageSlider />          
				</div>      
			</div>      
		</div>      
	)      
}      
  
export default Hero;  
  
