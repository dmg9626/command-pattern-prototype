﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

Checks player inventory for a projectile, then shoots it in direction player is facing.

Like the MoveCommand, this command only functions based on player input; a redesign is required to get this
working for NPC behavior.

 */

public class ShootCommand : Command {

    /// <summary>
    /// Bullet to be shot
    /// </summary>
    public Projectile projectile;

    /// <summary>
    /// Executes Weapon's PrimaryAttack() method
    /// <param name="actor">Actor to execute command on</param>
    /// </summary>
    public override void execute(Actor actor) 
    {
        if(Input.GetButtonDown("Shoot")) {
            Shoot(actor);
        }	
	}

    /// <summary>
    /// Checks actor's inventory for projectile, then shoots it from actor's position
    /// </summary>
    /// <param name="actor">Actor</param>
    private void Shoot(Actor actor)
    {
        GameController.Log("Executing ShootCommand on " + actor.name, GameController.LogCommands);

        // Retrieve projectile from actor inventory
        projectile = GetProjectile(actor);

        if(projectile != null) {
            // Set bullet on player
            projectile.transform.position = actor.transform.position;

            // Get direction actor is facing (TODO: consider handling this for actors that don't rotate)
            BaseConstants.Direction direction = actor.GetComponent<MoveComponent>().currentDirection;
            GameController.Log("Projectile direction: " + direction.ToString(), GameController.LogCommands);

            // Send projectile in that direction
            Vector2 trajectory = DirectionHelper.DirectionToVector(direction);
            projectile.Shoot(trajectory, actor);
        }
        else {
            GameController.LogWarning("Primary Shoot Command: could not find projectile in actor " + actor.name + "'s inventory", GameController.LogCommands);
        }
    }

    /// <summary>
    /// Searches actor's inventory for GameObject with Projectile component
    /// </summary>
    /// <param name="actor">Actor</param>
    /// <returns>Returns projectile if found, null otherwise</returns>
    private Projectile GetProjectile(Actor actor)
    {
        // Instantiate bullet (TODO: investigate why this causes a null reference)
        GameObject projectileObj = actor.inventory.Find(i => i.GetComponent<Projectile>() != null);
        if(projectileObj != null) {
            return GameObject.Instantiate(projectileObj).GetComponent<Projectile>();
        }
        else {
            return null;
        }
    }
}
