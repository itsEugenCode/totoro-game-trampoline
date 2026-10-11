# Design

## Context

Unity 6000.6.5f1. Сцена `Clearing`, префаб игрока, `core-movement` и `totoro-bounce` уже в основных спеках. См. `proposal.md`, раздел Why.

`PlayerMovement` уже рулит в воздухе: при команде `MoveTowards` к `moveInput * maxSpeed` с `airAcceleration`, без команды горизонталь не трогается. Земля использует тот же `maxSpeed`, плюс `groundAcceleration` / `groundDeceleration`. `PlayerVerticalImpulse` с `DefaultExecutionOrder(-50)` в `FixedUpdate` один раз выставляет `(horizontalSpeed, upwardSpeed)` после отскока; затем `PlayerMovement` пишет только новый `x` и сохраняет текущий `y`. `TotoroBounce` вызывает `Receive` из trigger-зоны белого живота.

## Goals / Non-Goals

**Goals:**

- Земля и воздух имеют независимые потолок скорости и ускорение в Inspector.
- Руление в воздухе одинаково доступно после прыжка с земли и после отскока.
- Вертикаль отскока и прыжка не затирается горизонтальной записью.
- Поведение остаётся в `PlayerMovement`, без второго писателя `velocity.x`.

**Non-Goals:**

- Новый компонент `AirController` или дублирование бега.
- Правка `TotoroBounce`, силы отскока, зоны живота, collectibles, UI, прогрессии, save, второго персонажа.
- Coyote time, буфер прыжка, air dash, переменная гравитация по высоте, переписывание камеры.
- Новые пакеты и GameManager.

## Decisions

- Capability не плодим: меняется только `core-movement`. Отдельный spec-файл `air-control` не нужен — это уточнение уже существующего Air control.
- Правка только в `PlayerMovement` и полях префаба. Ввод остаётся в `PlayerController` (`Horizontal`).
- Добавляется поле `airMaxSpeed` в Inspector. В воздухе цель скорости — `moveInput * airMaxSpeed`, на земле по-прежнему `moveInput * maxSpeed`. Так требование независимости закрывается без переименования уже настроенного `maxSpeed`.
- `airAcceleration` уже есть и остаётся полем воздуха. Отдельное воздушное торможение при отпущенной команде не вводится: без ввода горизонталь не меняется — это уже сценарий Keep momentum и совпадает с `totoro-bounce`.
- Запись скорости: в `FixedUpdate` после опоры считается только `nextSpeed` по `x`, затем `linearVelocity = (nextSpeed, linearVelocity.y)`. Вертикаль от `PlayerJump` и `PlayerVerticalImpulse` не трогается.
- `PlayerVerticalImpulse` и `TotoroBounce` не переписываются. Если после смены `airMaxSpeed` отскок потеряет `y`, сначала проверяют порядок кадров и сохранение `y`; точечный фикс импульса — только по факту регрессии.
- Стартовые значения подбирают в Play так, чтобы воздух был отзывчивее минимального ядра, но не обгонял землю без нужды. Конкретные числа — в Inspector, не в требованиях.
- Камера, земля, прыжок с земли и trigger-живот остаются как есть.

## Risks / Trade-offs

- [Два писателя `velocity.x` разойдутся] → воздух остаётся в `PlayerMovement`, отдельный компонент не создаём.
- [Общий `maxSpeed` оставить и «независимо» крутить только ускорение] → не закрывает требование отдельных потолков; нужен `airMaxSpeed`.
- [Переименовать `maxSpeed` в `groundMaxSpeed`] → лишний шум в уже настроенном префабе; оставляем `maxSpeed` для земли.
- [Руление в том же `FixedUpdate`, что и импульс, затрёт `y`] → движение пишет пару `(x, текущий y)`; импульс раньше по `DefaultExecutionOrder(-50)`.
- [Слишком высокий `airMaxSpeed` унесёт с дуги отскока] → потолок крутят в Inspector; сценарии проверяют и прыжок, и bounce.
- [Регрессия остановки на земле] → земная ветка с `groundDeceleration` не должна начать применяться в воздухе.

## Migration Plan

Сохранений нет. Откат — вернуть в `PlayerMovement` одну цель `maxSpeed` для воздуха и убрать `airMaxSpeed` с префаба. Bounce и камера не затрагиваются откатом.
