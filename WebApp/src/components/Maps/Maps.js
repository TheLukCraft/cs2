import React from "react";

import classes from "./Maps.module.css";

import Mirage from "../../img/Mirage.png";
import Inferno from "../../img/Inferno.png";
import Overpass from "../../img/Overpass.png";
import Ancient from "../../img/Ancient.png";
import Nuke from "../../img/Nuke.png";
import Vertigo from "../../img/Vertigo.png";
import Anubis from "../../img/Anubis.png";

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
        <img className={classes.img} src={Overpass} alt="Overpass Map" />
      </div>
      <div className={classes.image}>
        <img className={classes.img} src={Ancient} alt="Ancient Map" />
      </div>
      <div className={classes.image}>
        <img className={classes.img} src={Nuke} alt="Nuke Map" />
      </div>
      <div className={classes.image}>
        <img className={classes.img} src={Vertigo} alt="Vertigo Map" />
      </div>
      <div className={classes.image}>
        <img className={classes.img} src={Anubis} alt="Anubis Map" />
      </div>
    </div>
  );
}
