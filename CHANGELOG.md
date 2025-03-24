# Changelog

## [0.2.0] - Teleport System, Falling Animation & Environment Enhancements (2025-03-23)

### Added Features

- Implemented a teleport system that resets the player’s position when falling off the island.

- Added a falling animation to improve character responsiveness.

- Introduced a particle system for clouds to enhance environmental immersion.

- Upgraded visuals with a new HDR skybox.

- Placed new environment props (trees, rocks) to make the world feel more alive.

### Fixed

- Resolved an issue where the character moved twice due to incorrect input handling.

- Jump system adjusted to properly reset coyote time after landing.

- Fixed bouncing behavior when landing on the ground.

- Optimized gravity handling for a smoother falling experience.

- Ensured teleport preserves fall velocity to maintain animation consistency.

## [0.1.1] - Game Environment Update (2025-03-18)

### Added Features

- Fixed delay in player movement stopping when using the keyboard by replacing `Input.GetAxis` with `Input.GetAxisRaw`.
- Added 3D island elements to test player movement.

## [0.1.0] - Character Prototype (2025-03-16)

### Added Features

- Created the **Character Controller** for 3D movement.
- Implemented **keyboard + mouse** and **gamepad** support.
- Integrated a **3D character model** with basic animations.
- Added the following animations:
  - Running
  - Jumping in place
  - Jumping while running
- Configured **animation transitions** for seamless movement.
- Implemented **manual camera control** using both mouse and gamepad.
- Set up a **simple test environment** for prototyping.
