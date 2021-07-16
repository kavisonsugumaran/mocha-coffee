<template>
<div class="btn-link">
  <Button @click="onClick" :disabled="disabled" :class="['mocha-button', {'full-width' : isFullWidth}]" type="button">
    <slot></slot>
  </Button>
</div>
</template>

<script>
export default {
  name: "MochaButton",
  props: ['isFullWidth', 'disabled'],
  methods:{
    onClick() {
      this.$emit("click-event");
    }
  }

}
</script>

<style lang="scss" scoped>
@import "@/scss/global.scss";

.mocha-button {
  background: lighten($mocha-blue, 10%);
  color: white;
  padding: 0.8rem;
  transition: background-color 0.5s;
  margin: 0.3rem 0.2rem;
  display: inline-block;
  cursor: pointer;
  font-size: 1rem;
  min-width: 100px;
  border: none;
  border-bottom: 2px solid darken($mocha-blue, 20%);
  border-radius: 3px;

  &:hover {
    background: lighten($mocha-blue, 20%);
    transition: background-color 0.5s;
  }

  &:disabled {
    background: lighten($mocha-blue, 15%);
    transition: 2px solid lighten($mocha-blue, 20%);
  }

  &:active {
    background: $mocha-yellow;
    border-bottom: 2px solid lighten($mocha-yellow, 20%);
  }
}

.full-width {
  display: block;
  width: 100%;
}
</style>>
