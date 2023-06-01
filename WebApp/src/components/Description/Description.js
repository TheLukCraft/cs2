import React from "react";

import classes from "./Description.module.css";

export default function Description() {
  return (
    <div className={classes.flex}>
      <div className={classes.container}>
        <h2 className={classes.title}>
          Welcome to our training maps for Counter Strike 2
        </h2>
        <p>Choose your map for practice.</p>
      </div>
    </div>
  );
}
