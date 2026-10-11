# Proposal

## Why

Базовая камера из `core-movement` уже тянется за целью по обеим осям, но при высоком отскоке от живота не хватает запаса кадра сверху, а у земли любое мелкое движение сразу двигает камеру. Нужно спокойное вертикальное сопровождение под прыжок и bounce, без Cinemachine и без правок физики игрока.

## What Changes

- Существующий `CameraController` на `Main Camera` дорабатывается: плавное следование по X и Y, вертикальный запас при подъёме, менее нервная реакция у земли.
- Сглаживание и параметры вертикального сопровождения задаются в Inspector.
- Опциональные границы кадра уровня можно задать на том же компоненте без отдельной подсистемы; пока границ сцены нет — поля есть, зажим можно оставить выключенным.
- Вторая камера, Cinemachine, кат-сцены и режимы переключения не появляются.
- `PlayerMovement`, `PlayerJump`, `PlayerVerticalImpulse`, `TotoroBounce` и collectibles не переписываются.

## Capabilities

### New Capabilities

- `vertical-camera`: плавное горизонтальное и вертикальное сопровождение с запасом сверху на высоком отскоке, спокойствием у земли, настройками в Inspector и заделом под границы уровня.

### Modified Capabilities

Нет. Требования `core-movement`, `totoro-bounce` и `collectibles` не меняются: этот change усиливает поведение камеры поверх уже описанного следования.

### Dependencies

- `core-movement`: цель камеры и базовое следование в `openspec/specs/core-movement/spec.md`.
- `totoro-bounce`: высокий отскок, который камера должна держать в кадре (`openspec/specs/totoro-bounce/spec.md`).

## Impact

- В основном `unity/Assets/Scripts/Camera/CameraController.cs` и поля `Main Camera` в `unity/Assets/Scenes/Clearing.unity`.
- Orthographic size камеры может подкрутиться в Inspector, если текущего кадра мало для дуги отскока — это настройка камеры, не физика игрока.
- Пакеты не добавляются. Cinemachine в проекте нет и не ставится.
