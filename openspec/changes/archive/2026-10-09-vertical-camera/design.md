# Design

## Context

Unity 6000.6.5f1. Активных change нет. В основных спеках: `core-movement`, `totoro-bounce`, `collectibles`; `air-control` уже влит в `core-movement`. См. `proposal.md`, раздел Why.

Факт по проекту (проверено):

- Одна orthographic `Main Camera`, size 5, фон мягкий зелёный.
- `CameraController`: `target` = игрок, `offset (0, 1, -10)`, один `smoothTime 0.15`, `SmoothDamp` по обеим осям в `LateUpdate`, snap в `Start`.
- Cinemachine и другие camera-пакеты в `manifest.json` нет.
- Отскок `upwardSpeed` 24 при `gravityScale` 3 даёт пик стоп примерно y ≈ 12–13; collectibles разложены по дуге над животом.
- Отдельной системы границ уровня нет. Земля шириной 24.
- Игрок пишет скорость в своих компонентах; камера цель только читает `Transform`.

## Goals / Non-Goals

**Goals:**

- Спокойная камера у земли и запас сверху на высоком отскоке.
- Настройки вертикали и сглаживания в Inspector.
- Задел под clamp границ без отдельного менеджера.
- Только доработка существующего `CameraController`.

**Non-Goals:**

- Cinemachine, вторая камера, кат-сцены, переключение камер, aim/zoom-режимы.
- Правки `PlayerMovement`, `PlayerJump`, `PlayerVerticalImpulse`, `TotoroBounce`, collectibles.
- Полноценный level streaming / room system.
- Параллакс леса (отдельный будущий change).

## Decisions

- Capability новая: `vertical-camera`. Базовое «Camera follow» в `core-movement` и «Camera follows the high bounce» в `totoro-bounce` не переписываем — они остаются минимальным контрактом; этот change добавляет качество вертикали.
- Один файл логики: `CameraController`. Вторая камера и новый сервис не нужны.
- Вертикаль отделяется от горизонтали в настройках: как минимум отдельные `smoothTime` по осям или эквивалент (горизонталь/вертикаль), чтобы у земли и в полёте не делить одно «общее» ощущение.
- Спокойствие у земли — вертикальная dead zone (полоса вокруг текущей точки следования): пока цель внутри полосы, желаемая Y камеры не двигается; при выходе — догоняет край/цель. Так мелкий bob и короткий прыжок не дёргают кадр.
- Запас сверху на подъёме — вертикальный look-ahead / bias, завязанный на подъём (например `target` velocity.y выше порога или просто смещение offset.y вверх, пока цель выше dead zone). Читаем `Rigidbody2D.linearVelocity` только для смещения камеры, скорость тела не пишем.
- Плавность — по-прежнему `SmoothDamp` (или раздельные damp по осям). Стартовый `SnapToTarget` в `Start` сохраняется, чтобы первый кадр не «плыл» из пустоты.
- Границы: опциональные `min`/`max` мировых координат центра камеры (или краёв вида) и флаг `useBounds`. Пока в `Clearing` можно задать черновой clamp по ширине земли и по низу, верх оставить свободным под bounce, либо оставить `useBounds = false` и только поля под будущее. Отдельный `CameraBounds` volume не обязателен в этом change; при необходимости один BoxCollider-trigger позже можно повесить без смены API камеры.
- Orthographic size: если при текущем size 5 персонаж на пике отскока жмётся к верху даже с look-ahead — поднять size или offset в Inspector на `Main Camera`, не трогая импульс bounce.
- Физику игрока не трогаем: камера только `transform.position` в `LateUpdate`.

## Risks / Trade-offs

- [Слишком большая dead zone «теряет» короткий прыжок] → высота зоны в Inspector, проверка обычного прыжка в Play.
- [Look-ahead опаздывает на резком bounce] → bias по скорости вверх и/или чуть меньший vertical smoothTime; не мгновенный snap.
- [Clamp обрезает небо на пике] → верхнюю границу не зажимать жёстко, пока уровня-потолка нет; или поднять max под дугу.
- [Чтение Rigidbody на камере связывает слои] → только read velocity для bias; fallback на чистый Transform, если body нет.
- [Дублирование требований с core-movement] → новая capability описывает качество вертикали; минимальное follow в core остаётся.
- [Cinemachine «на будущее»] → отвергаем в этом change: в проекте её нет, согласования на пакет нет.

## Migration Plan

Откат — вернуть `CameraController` к одному `smoothTime` + `offset` без dead zone / look-ahead / bounds и прежние поля на `Main Camera`. Скрипты игрока и bounce не затрагиваются.
