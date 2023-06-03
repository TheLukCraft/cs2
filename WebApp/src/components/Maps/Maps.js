import React from "react";

import classes from "./Maps.module.css";

import Mirage from "../../img/Mirage.png";
import Inferno from "../../img/Inferno.png";

export default function Maps() {
  return (
    <div className={classes.container}>
      <div className={classes.image}>
        <img className={classes.img} src={Mirage} alt="Mirage Map" />
      </div>
      <div className={classes.image}>
        <img className={classes.img} src={Inferno} alt="Inferno Map" />
      </div>
      <div className={classes.image}>
        <img className={classes.img} src={Inferno} alt="Inferno Map" />
      </div>
      <div className={classes.image}>
        <img className={classes.img} src={Inferno} alt="Inferno Map" />
      </div>
      <div className={classes.image}>
        <img className={classes.img} src={Inferno} alt="Inferno Map" />
      </div>
      <div className={classes.image}>
        <img className={classes.img} src={Inferno} alt="Inferno Map" />
      </div>
    </div>
  );
}
