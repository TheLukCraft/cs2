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
      <div className={classes.position}>
        <div className={classes.imageMirage}>
          <img className={classes.img} src={Mirage} alt="Mirage Map" />
        </div>
      </div>
      <div className={classes.position}>
        <div className={classes.imageInferno}>
          <img className={classes.img} src={Inferno} alt="Inferno Map" />
        </div>
      </div>
      <div className={classes.position}>
        <div className={classes.imageOverpass}>
          <img className={classes.img} src={Overpass} alt="Overpass Map" />
        </div>
      </div>
      <div className={classes.position}>
        <div className={classes.imageAncient}>
          <img className={classes.img} src={Ancient} alt="Ancient Map" />
        </div>
      </div>
      <div className={classes.position}>
        <div className={classes.imageNuke}>
          <img className={classes.img} src={Nuke} alt="Nuke Map" />
        </div>
      </div>
      <div className={classes.position}>
        <div className={classes.imageVertigo}>
          <img className={classes.img} src={Vertigo} alt="Vertigo Map" />
        </div>
      </div>
      <div className={classes.position}>
        <div className={classes.imageAnubis}>
          <img className={classes.img} src={Anubis} alt="Anubis Map" />
        </div>
      </div>
    </div>
  );
}
